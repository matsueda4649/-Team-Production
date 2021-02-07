using ManagerSystem;

public interface IUpdatable
{
    void UpdateMe();

    void OnEnable();

    void OnDisable();
}

public interface IFixedUpdatable
{
    void FixedUpdateMe();

    void OnEnable();

    void OnDisable();
}