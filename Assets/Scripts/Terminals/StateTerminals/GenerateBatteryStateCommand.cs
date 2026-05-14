using UnityEngine;

public class GenerateBatteryStateCommand : StateCommand
{
    [Header("Terminal Target")]
    [SerializeField] private BatteryGenerator generator;

    protected override void OnExecute()
    {
        generator.DestroyBattery();
    }

    void Update(){
        base.Update();
        sameState = !generator.HasLivingBattery();
    }
}
