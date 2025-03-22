using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{
    public List<Shape> shapes;
    public int spawnTime = 2;
    public Transform destination;
    
    private void Start()
    {
        StartCoroutine(SpawnShapes());
    }
    
    private IEnumerator SpawnShapes()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            int randomShapeIndex = Random.Range(0, shapes.Count);
            Shape shape = shapes[randomShapeIndex];
            GameObject newShape = Instantiate(shape.gameObject, transform.position, Quaternion.identity);
            var shapeComponent = newShape.GetComponent<Shape>();
            
            //give the shape a Destination
            shapeComponent.destination = destination;
            shapeComponent.travelling = true;
        }
    }
}
