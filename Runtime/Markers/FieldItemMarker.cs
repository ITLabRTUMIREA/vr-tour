using Excursion360_Builder.Shared.States.Items.Field;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Excursion360_Builder.Runtime.Markers
{
    public class FieldItemMarker : Marker
    {
        public FieldItem fieldItem;
        public override string Title => fieldItem.title;
        public GameObject fieldItemContainerPrefab;

        public Material fieldItemMaterial;

        private MeshRenderer meshRenderer;

        private void Update()
        {
            if (meshRenderer != null) {
                if (hovered)
                {
                    meshRenderer.enabled = true;
                }
                else
                {
                    meshRenderer.enabled = false;
                }
            }
        }

        public override void HandleInteract()
        {
            if (transform.childCount == 0) {
                var fieldItemContainer = Instantiate(fieldItemContainerPrefab, this.gameObject.transform);
                fieldItemContainer.GetComponent<fieldItemContent>().Init(fieldItem);
                fieldItemContainer.transform.position = new Vector3(meshRenderer.bounds.center.x, 0, meshRenderer.bounds.center.z);
            }
        }

        public void Init(FieldItem fieldItem)
        {
            this.fieldItem = fieldItem;
            var vertices = new Vector3[]
            {
                fieldItem.vertices[0].Orientation * Vector3.forward,
                fieldItem.vertices[1].Orientation * Vector3.forward,
                fieldItem.vertices[2].Orientation * Vector3.forward,
                fieldItem.vertices[3].Orientation * Vector3.forward
            };
            var tris = new int[]
            {
                0,1,2,
                0,2,3
            };
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = fieldItemMaterial;
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            Mesh mesh = new Mesh
            {
                vertices = vertices,
                triangles = tris
            };
            meshFilter.mesh = mesh;
            gameObject.AddComponent<MeshCollider>();
        }
    }
}
