namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

public class LinkTargetIdList
{
    public short IdListSize { get; private set; }
    public List<IdList> IdLists { get; } = new List<IdList>();
    public static LinkTargetIdList ReadFrom(BinaryReader reader)
    {
        LinkTargetIdList targetIdList = new LinkTargetIdList();
        targetIdList.IdListSize = reader.ReadInt16();
        IdList idList = new IdList();
        int readSize = 0;
        while (readSize < targetIdList.IdListSize)
        {
            short currentElementSize = reader.ReadInt16();
            readSize += 2;
            if (currentElementSize == 0)
            {
                targetIdList.IdLists.Add(idList);
                idList = new IdList();
                continue;
            }

            byte[] data = reader.ReadBytes(currentElementSize);
            readSize += currentElementSize;
            
            idList.IdItems.Add(new IdItem(currentElementSize, data));
        }
        
        return targetIdList;
    }
}