using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice And Fire Effect", menuName = "Data/Item Effect/Ice And Fire")]
public class IceAndFire_Effect_SO : ItemEffect_SO
{
    [SerializeField] GameObject iceAndFirePrefab;
    [SerializeField] float xVelocity;

    public override void ExecuteEffect(Transform _spawnTransform)
    {
        Player player = PlayerManager.Instance.player;

        bool isThirdCombo = player.primaryAttackState.comboCounter == 2;

        if (isThirdCombo)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _spawnTransform.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir, 0);
        }
    }
}
