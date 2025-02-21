namespace YAGO.World.Host.Database.Events
{
    public enum EventParticipantType
    {
        //Not matter
        Main = 1,
        Target = 2,

        //Vasal-Suzerain
        Vasal = 1001,
        Suzerain = 1002,

        //War
        Agressor = 2001,
        Defender = 2002,
        SupporetForDefender = 2203,
        SupporetForAgressor = 2204
    }
}
