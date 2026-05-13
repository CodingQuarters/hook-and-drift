using UnityEngine;

public class GrapplerLine : MonoBehaviour 
{ // The opening brace must come BEFORE your variables
    public GameObject linePrefab; // Prefab for the line renderer
    public void Animate(float targetLength, float extendTime = 0.15f, float holdTime = 0.3f)
    {
        Debug.Log("Animating line with target length: " + targetLength);
        StartCoroutine(AnimateLine(targetLength, extendTime, holdTime));
    }

    private System.Collections.IEnumerator AnimateLine(float targetLength, float extendTime, float holdTime)
    {
        float elapsedTime = 0f;
        Vector3 initialScale = transform.localScale;

        // Extend the line
        while (elapsedTime < extendTime)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / extendTime);
            transform.localScale = new Vector3(initialScale.x, targetLength * t, initialScale.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = new Vector3(initialScale.x, targetLength, initialScale.z);

        // Hold the line at full length
        yield return new WaitForSeconds(holdTime);

        // Retract the line
        elapsedTime = 0f;
        while (elapsedTime < extendTime)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / extendTime);
            transform.localScale = new Vector3(initialScale.x, targetLength * (1f - t), initialScale.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        Destroy(gameObject); // Destroy the line after animation is complete
    }   
}
