using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [System.Serializable]
    public class UIItemVariable
    {
        public Transform Target;
#if UNITY_EDITOR
        public Object[] Components;
        public string[] VarNameArray;
#endif
    }

    public class UIItemReference : MonoBehaviour
    {
        public List<UIItemVariable> itemVarList;

        public bool IsExistItem(Transform target)
        {
            if (itemVarList != null && itemVarList.Count > 0)
            {
                foreach (var item in itemVarList)
                {
                    if (target == item.Target)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public UIItemVariable GetItem(Transform target)
        {

            if (itemVarList == null)
            {
                itemVarList = new List<UIItemVariable>();
            }

            foreach (var item in itemVarList)
            {
                if (target == item.Target)
                {
                    return item;
                }
            }
            var newItem = new UIItemVariable();
            newItem.Target = target;
            itemVarList.Add(newItem);
            return newItem;
        }

        public void RemoveItem(UIItemVariable item)
        {
            if (itemVarList != null && itemVarList.Count > 0)
            {
                itemVarList.Remove(item);
            }
        }

#if UNITY_EDITOR
        public void ExchangeItemPos(int posA, int posB)
        {
            var tmpNode = itemVarList[posA];
            itemVarList[posA] = itemVarList[posB];
            itemVarList[posB] = tmpNode;
        }

        public void MoveToTargetPos(int posA, int posB)
        {
            if (posA == posB || posA >= itemVarList.Count || posB >= itemVarList.Count)
            {
                return;
            }

            if (posA < posB)
            {
                var tmpNode = itemVarList[posA];
                for (int i = posA; i < posB; i++)
                {
                    itemVarList[i] = itemVarList[i + 1];
                }
                itemVarList[posB] = tmpNode;
            }
            else
            {
                var tmpNode = itemVarList[posA];

                for (int i = posA; i > posB; i--)
                {
                    itemVarList[i] = itemVarList[i - 1];
                }
                itemVarList[posB] = tmpNode;
            }

        }
#endif
    }