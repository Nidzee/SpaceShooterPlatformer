using System;

public abstract class Manager<T> : IManager  where T : class, IManager, new() {
    
    protected static T instance;
    public Guid instanceId { get; } = Guid.NewGuid();




    public abstract void init();

    
    public static T Instance
    {
        get 
        {
            if (instance == null) 
            {
                instance = new T();
            }

            return instance;
        }
    }
}