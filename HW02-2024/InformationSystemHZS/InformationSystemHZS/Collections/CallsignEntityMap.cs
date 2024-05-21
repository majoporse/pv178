using InformationSystemHZS.Models.HelperModels;
using InformationSystemHZS.Models.Interfaces;
using InformationSystemHZS.Services;
using System.Runtime.InteropServices.Marshalling;

namespace InformationSystemHZS.Collections;

/// <summary>
/// Stores and manages data that maps valid callsigns to entities of a given type. 
/// also changes the objects inside the reference list for potentially saving to json
/// has to have set a parent before using
/// </summary>
/// <typeparam name="T">IBaseModel</typeparam>
public class CallsignEntityMap<T> where T : IBaseModel
{
    Dictionary<string, T> _callsignEntityMap = new Dictionary<string, T>();

    public CallsignEntityMap(List<T> objs)
    {
        foreach (var obj in objs)
        {
            SafelyAddEntity(obj, obj.Callsign);
        }
    }

    /// <summary>
    /// Returns an entity based on the given callsign.
    /// If the entity does not exist then returns default
    /// (see: https://learn.microsoft.com/cs-cz/dotnet/csharp/language-reference/operators/default).
    /// </summary>
    public T? GetEntity(string callsign) =>
        _callsignEntityMap.TryGetValue(callsign, out T val) ? val : default;

    /// <summary>
    /// Returns all mambers of the map.
    /// </summary>
    public List<T> GetAllEntities() =>
        _callsignEntityMap.Values.ToList();

    /// <summary>
    /// Returns the total number of entities in the map.
    /// </summary>
    public int GetEntitiesCount() =>
        _callsignEntityMap.Count;

    /// <summary>
    /// Tries to safely add an entity. If callsign already exists within this map
    /// or is not in a valid format (i.e. S01, H01, J01, ...), returns false.
    /// Otherwise adds an entity to this map and returns true.
    /// If no callsign is provided, it generates a new one by incrementing the current 
    /// highest callsign by 1 (i.e. generates S04, if highest available is S03).
    /// </summary>
    public bool SafelyAddEntity(T entity, string? callsign = null)
    {
        callsign = callsign ?? CallSignGenerator.GenerateCallSign(_callsignEntityMap.Values.ToList());
        entity.Callsign = callsign;

        if (callsign == null || !ValidationService.HasValidCallsign(entity) || _callsignEntityMap.ContainsKey(callsign))
            return false;

        entity.Callsign = callsign;

        _callsignEntityMap.Add(callsign, entity);
        return true;
    }

    /// <summary>
    /// Tries to safely remove an entity from this map. 
    /// If it does not exist in this map, return false.
    /// If it exists, remove it from this map and return true.
    /// </summary>
    public bool SafelyRemoveEntity(string callsign) =>
        _callsignEntityMap.Remove(callsign);
}
