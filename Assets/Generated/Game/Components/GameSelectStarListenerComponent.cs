//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public SelectStarListenerComponent selectStarListener { get { return (SelectStarListenerComponent)GetComponent(GameComponentsLookup.SelectStarListener); } }
    public bool hasSelectStarListener { get { return HasComponent(GameComponentsLookup.SelectStarListener); } }

    public void AddSelectStarListener(System.Collections.Generic.List<ISelectStarListener> newValue) {
        var index = GameComponentsLookup.SelectStarListener;
        var component = (SelectStarListenerComponent)CreateComponent(index, typeof(SelectStarListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceSelectStarListener(System.Collections.Generic.List<ISelectStarListener> newValue) {
        var index = GameComponentsLookup.SelectStarListener;
        var component = (SelectStarListenerComponent)CreateComponent(index, typeof(SelectStarListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveSelectStarListener() {
        RemoveComponent(GameComponentsLookup.SelectStarListener);
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

    static Entitas.IMatcher<GameEntity> _matcherSelectStarListener;

    public static Entitas.IMatcher<GameEntity> SelectStarListener {
        get {
            if (_matcherSelectStarListener == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.SelectStarListener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherSelectStarListener = matcher;
            }

            return _matcherSelectStarListener;
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EventEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public void AddSelectStarListener(ISelectStarListener value) {
        var listeners = hasSelectStarListener
            ? selectStarListener.value
            : new System.Collections.Generic.List<ISelectStarListener>();
        listeners.Add(value);
        ReplaceSelectStarListener(listeners);
    }

    public void RemoveSelectStarListener(ISelectStarListener value, bool removeComponentWhenEmpty = true) {
        var listeners = selectStarListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0) {
            RemoveSelectStarListener();
        } else {
            ReplaceSelectStarListener(listeners);
        }
    }
}
