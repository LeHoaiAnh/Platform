using Goap.Classes;
using Goap.Classes.References;
using Goap.Interfaces;
using Goap.Sensors;

public class UsingSkillSensor : LocalTargetSensorBase
{
    public override void Created()
    {
    }

    public override void Update()
    {
    }
    
    public override ITarget Sense(IMonoAgent agent, IComponentReference references)
    {
        var pos = agent.transform.position;

        return new PositionTarget(pos);
    }


}
