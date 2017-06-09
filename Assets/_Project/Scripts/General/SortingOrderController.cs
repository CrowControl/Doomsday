using UnityEngine;
using UnityEngine.Rendering;

namespace _Project.Scripts.General
{
    [ExecuteInEditMode]
    public class SortingOrderController : MonoBehaviour
    {
        #region Editor Variables
        [SerializeField] private bool _disableAfterAwake;
        #endregion

        #region Components
        private ISortingOrderSource _sortingOrderSource;
        #endregion
    
        private void Awake ()
        {
            //Get the sorting order source, and update once.
            InitializeSortingOrderSource();
            Update();

            //If we're in the editor, stop there.
            #if UNITY_EDITOR
            if (!Application.isPlaying) return; 
            #endif

            //If we're playing, disable if we want.
            if (_disableAfterAwake)
                enabled = false;
        }

        // Update is called once per frame
        private void Update()
        {
            _sortingOrderSource.SortingOrder = (int)(transform.position.y * -10);
        }

        #region Getting the sorting-order-source component.

        private void InitializeSortingOrderSource()
        {
            SortingGroup group = GetComponentInParent<SortingGroup>();
            if (group != null)
                _sortingOrderSource = new GroupSortingOrder(group);

            else
            {
                SpriteRenderer[] renderers = GetComponentsInParent<SpriteRenderer>();
                _sortingOrderSource = new RenderersSortingOrder(renderers);
            }

            
        }

        #region Sorting Order Source Types
        private interface ISortingOrderSource
        {
            int SortingOrder { get; set; }
            int SortingLayerId { get; set; }
        }

        private class GroupSortingOrder : ISortingOrderSource
        {
            private readonly SortingGroup _group;

            public GroupSortingOrder(SortingGroup group)
            {
                _group = group;
            }

            public int SortingOrder
            {
                get { return _group.sortingOrder; }
                set { _group.sortingOrder = value; }
            }

            public int SortingLayerId
            {
                get { return _group.sortingLayerID; }
                set { _group.sortingLayerID = value; }
            }
        }

        private class RenderersSortingOrder : ISortingOrderSource
        {
            private readonly SpriteRenderer[] _renderers;

            public RenderersSortingOrder(SpriteRenderer[] renderers)
            {
                _renderers = renderers;
            }

            public int SortingOrder
            {
                get
                {
                    return _renderers[0].sortingOrder;
                }
                set
                {
                    foreach (SpriteRenderer renderer in _renderers)
                        renderer.sortingOrder = value;
                }
            }

            public int SortingLayerId
            {
                get
                {
                    return _renderers[0].sortingLayerID;
                }
                set
                {
                    foreach (SpriteRenderer renderer in _renderers)
                        renderer.sortingLayerID = value;
                }
            }
        }
        #endregion
        #endregion
    }
}
