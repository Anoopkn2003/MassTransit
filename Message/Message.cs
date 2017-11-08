namespace Message
{
    public interface IRegisterTeamMember
    {
        string MockyURL { get; }

    }

    public interface ITeamMemberRegistered
    {
        int Id { get; }
        string Name { get; }

    }
}
