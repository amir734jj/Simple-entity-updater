# Simple-Entity-Updater

This is similar to [this](https://github.com/amir734jj/Entity-updater) project but without any expression tree or reflection.


### Example

Mapper profile:

```csharp
public class DummyClassProfile : AbstractProfile<DummyClass>
{
    public DummyClassProfile()
    {
        Map(x => x.Firstname)
            .DefaultComparator()
            .Assignment((x, y) => x.Firstname = y);
        
        Map(x => x.Lastname)
            .DefaultComparator()
            .Assignment((x, y) => x.Lastname = y);
    }
}
```

Use:
```csharp
   
var mapper = EntityUpdater.New(x => x.Assembly(typeof(EntityUpdaterTest).Assembly));

DummyClass source = ...;
DummyClass destination = ...;

_mapper.Map(source, destination);

```
