//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly CheckEndComponent checkEndComponent = new CheckEndComponent();

    public bool isCheckEnd {
        get { return HasComponent(GameComponentsLookup.CheckEnd); }
        set {
            if (value != isCheckEnd) {
                var index = GameComponentsLookup.CheckEnd;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : checkEndComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherCheckEnd;

    public static Entitas.IMatcher<GameEntity> CheckEnd {
        get {
            if (_matcherCheckEnd == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.CheckEnd);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCheckEnd = matcher;
            }

            return _matcherCheckEnd;
        }
    }
}
