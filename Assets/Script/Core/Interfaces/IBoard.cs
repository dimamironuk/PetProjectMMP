using System.Collections.Generic;

public interface IBoard
{
    IEnumerable<CharacterUnit> Characters { get; }
    bool TryGetFreeSlot(out CharacterSlot slot);
    bool TryDeploy(CharacterCardData data);
    bool HasSlot(CharacterSlot slot);
    bool HasUnit(CharacterUnit unit);
}