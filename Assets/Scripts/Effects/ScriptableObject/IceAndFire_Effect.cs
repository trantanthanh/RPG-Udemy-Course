using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice And Fire Effect", menuName = "Data/Item Effect/Ice And Fire")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] GameObject iceAndFirePrefab;
    [SerializeField] Vector2 velocity;

    public override void ExecuteEffect(Transform _spawnTransform)
    {
        Player player = PlayerManager.Instance.player;

        bool isThirdCombo = player.primaryAttackState.comboCounter == 2;

        if (isThirdCombo)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _spawnTransform.position, player.transform.rotation);
            velocity.x = player.facingDir * Mathf.Abs(velocity.x);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = velocity;
        }
    }
}
