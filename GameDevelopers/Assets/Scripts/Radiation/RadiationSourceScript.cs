using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class RadiationSourceScript : MonoBehaviour
{
    //private List<Line> all_walls;
    private Vector2 radiationSourcePosition;
    private List<GameObject> needToCheckObjects;

    public float radiationPower;
    public float minValueToBecomeSource;
    public float minTimeToBecomeSource;
    public Light2D red_effect;

    public bool alwaysRadioactive;
    public float lifeTime;

    private float radius;
    private float start_time, current_time;
    void Start()
    {
        if (!alwaysRadioactive)
            start_time = Time.time;
        needToCheckObjects = new List<GameObject>();
        //all_walls = GameObject.Find("Walls").GetComponent<TestWallsScript>().GetAllWalls();
        radiationSourcePosition = new Vector2(transform.position.x, transform.position.y);

        radius = gameObject.GetComponent<CircleCollider2D>().radius;
    }

    void FixedUpdate()
    {
        if (!alwaysRadioactive)
        {
            current_time += Time.fixedDeltaTime;
            if ((current_time - start_time) > lifeTime)
            {
                Destroy(gameObject.GetComponent<CircleCollider2D>());
                Destroy(gameObject.GetComponent<RadiationSourceScript>());
                Destroy(gameObject.GetComponent<Light2D>());
            }
        }
        foreach (GameObject needToCheck in needToCheckObjects) {
            float radiationChangeKoef = 1f;
            // Сначала проверим все стены, защищают ли они полученный объект от радиации
            RaycastHit2D[] raycast_results;
            raycast_results = Physics2D.RaycastAll(radiationSourcePosition, needToCheck.transform.position);

            Vector2 toNeedToCheck = new Vector2(needToCheck.transform.position.x, needToCheck.transform.position.y) - radiationSourcePosition;

            if (raycast_results.Length > 2)
            {
                bool flag = false;
                foreach (RaycastHit2D raycast_result in raycast_results)
                {
                    if (raycast_result.transform != gameObject.transform && raycast_result.transform != needToCheck.transform)
                    {
                        Vector2 raycastResultPos = new Vector2(raycast_result.transform.position.x, raycast_result.transform.position.y);
                        Vector2 toRaycastResult = raycastResultPos - radiationSourcePosition;

                        print((toRaycastResult.sqrMagnitude, toNeedToCheck.sqrMagnitude));

                        if (toRaycastResult.sqrMagnitude <= toNeedToCheck.sqrMagnitude)
                        {
                            // Если объект ближе, чем стена, то менять коэффициент не нужно, поэтому рассматриваем только обратную ситуацию
                            // Но в случае, если хотя бы одна стена закрывает объект, то менять радиацию не надо, для этого используем флаг
                            flag = true;
                            try
                            {
                                // Стена настроена, то есть имеет свой уровень поглощения радиации
                                RadiationAffected radiationAffectedRef = raycast_result.rigidbody.gameObject.GetComponent<RadiationAffected>();
                                radiationChangeKoef *= radiationAffectedRef.radiationCoef;
                            }
                            catch
                            {
                                // Стена полностью поглощает радиацию
                            }
                        }
                        Debug.DrawRay(radiationSourcePosition, toRaycastResult, Color.red);
                        Debug.DrawRay(radiationSourcePosition, toNeedToCheck, Color.yellow);
                    }
                }
                if (!flag)
                {
                    radiationChangeKoef = 0f;
                }
            }
            radiationChangeKoef *= (1 - toNeedToCheck.magnitude / radius);
            float radiationToAffect = Mathf.Clamp(0f, radiationPower * radiationChangeKoef, radiationPower);

            if (radiationToAffect > minValueToBecomeSource)
            {
                try
                {
                    RadiationSourceScript needComponent;
                    needToCheck.TryGetComponent<RadiationSourceScript>(out needComponent);
                    // Если получилось получить компонент, значит объект уже источник радиации и новый добавлять не нужно
                    if (needComponent == null)
                    {
                        throw new KeyNotFoundException();
                    }
                }
                catch
                {
                    CircleCollider2D new_trigger_collider = needToCheck.AddComponent<CircleCollider2D>();
                    new_trigger_collider.isTrigger = true;
                    new_trigger_collider.radius = radius / 2;
                    RadiationSourceScript component = needToCheck.AddComponent<RadiationSourceScript>();
                    component.alwaysRadioactive = false;
                    component.lifeTime = 10f;
                    component.minValueToBecomeSource = radiationPower / 4;
                    component.radiationPower = radiationPower / 2;
                    Light2D effect = needToCheck.AddComponent<Light2D>();
                    effect.lightType = red_effect.lightType;
                    effect.pointLightInnerRadius = red_effect.pointLightInnerRadius;
                    effect.pointLightOuterRadius = red_effect.pointLightOuterRadius;
                    //effect.falloffIntensity = red_effect.falloffIntensity;
                    effect.color = red_effect.color;
                    effect.intensity = red_effect.intensity;
                    //effect.volumeOpacity = red_effect.volumeOpacity;
                    effect.shadowIntensity = red_effect.shadowIntensity;
                }
            }

            if (needToCheck.name == "player")
            {
                // Игрок в радиоактивной зоне
                needToCheck.GetComponent<Player>().health -= radiationToAffect;
                //print(radiationToAffect);
            }
            else if (needToCheck.CompareTag("Enemy"))
            {
                // Противник в радиоактивной зоне. Если это нищий, то он сразу умирает от радиации, если полицейский, то у него иммунитет, в остальных случаях снимаем здоровье
                Enemy enemyScript = needToCheck.GetComponent<Enemy>();
                int npc_index = enemyScript.npc_index;
                if (npc_index == 1)
                {
                    Destroy(needToCheck);
                } else if (npc_index != 0)
                {
                    enemyScript.health -= radiationToAffect;
                }
            }
            // print(radiationToAffect);
        }
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        // Объект в зоне радиации
        *//*RaycastHit2D[] raycast_results;
        raycast_results = Physics2D.RaycastAll(radiationSourcePosition, collision.transform.position);
        foreach (RaycastHit2D raycast_result in raycast_results)
        {
            if (raycast_result.transform != gameObject.transform)
            {
                Vector2 toRaycastResult = new Vector2(raycast_result.transform.position.x, raycast_result.transform.position.y);
                Debug.DrawRay(radiationSourcePosition, toRaycastResult, Color.yellow);
                bool walls_checked = CheckWalls(toRaycastResult);
            }
        }*//*
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>().bodyType.Equals(RigidbodyType2D.Dynamic))
        {
            needToCheckObjects.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (needToCheckObjects.Contains(collision.gameObject))
        {
            needToCheckObjects.Remove(collision.gameObject);
        }
    }

/*    private bool CheckWalls(Vector2 toRaycastResult)
    {
        bool flag = false;
        foreach (Line wall in all_walls)
        {
            // Позиция игрока
            Vector2 playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);

            // Крайние точки стены:
            Vector2 wallA = new Vector2((float)wall.x1, (float)wall.y1);
            Vector2 wallB = new Vector2((float)wall.x2, (float)wall.y2);

            // Векторы до точек стены:
            Vector2 toWallA = wallA - radiationSourcePosition;
            Vector2 toWallB = wallB - radiationSourcePosition;

            // Рисуем лучи до крайних точек стены:
            //Debug.DrawRay(radiationSourcePosition, toWallA, Color.yellow);
            //Debug.DrawRay(radiationSourcePosition, toWallB, Color.yellow);

            //Ray2D toWallARay = new Ray2D(wallA, wallB);
            //Ray2D toPlayer = new Ray2D(radiationSourcePosition, playerPosition);

            float angleWallARaycastResult = Vector2.Angle(toRaycastResult, toWallA);
            float angleWallBRaycastResult = Vector2.Angle(toRaycastResult, toWallB);

            print((angleWallARaycastResult, angleWallBRaycastResult));

            // (Physics2D.Raycast(radiationSourcePosition, playerPosition).transform != player.transform)

            if (((angleWallARaycastResult * angleWallBRaycastResult) < 0))
            {
                flag = true; // Объект спрятался за стенкой
                break;
            }
        }
        return flag;
    }*/
}
