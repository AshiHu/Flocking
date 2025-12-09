using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public FishData data;

    public Vector2 acceleration;
    public Vector2 velocity;

    private void Start()
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    private void Update()
    {
        var boidColliders = Physics2D.OverlapCircleAll(transform.position, data.neighborhoodRadius);
        var boids = boidColliders.Select(boidCollider => boidCollider.GetComponent<Fish>()).ToList();
        boids.Remove(this);

        ComputeAcceleration(boids);
        UpdateVelocity();
        UpdatePosition();
        UpdateRotation();
    }

    private void ComputeAcceleration(IEnumerable<Fish> boids)
    {
        var alignment = Alignment(boids);
        var separation = Separation(boids);
        var cohesion = Cohesion(boids);

        acceleration = data.alignmentAmount * alignment + data.cohesionAmount * cohesion + data.separationAmount * separation;
    }

    public void UpdateVelocity()
    {
        velocity += acceleration;
        velocity = LimitMagnitude(velocity, data.maxSpeed);
    }

    private void UpdatePosition()
    {
        transform.Translate(velocity * Time.deltaTime, Space.World);
    }

    private void UpdateRotation()
    {
        var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private Vector2 Alignment(IEnumerable<Fish> boids)
    {
        var velocity = Vector2.zero;
        if (!boids.Any()) return velocity;

        foreach (var boid in boids)
        {
            velocity += boid.velocity;
        }

        velocity /= boids.Count();
        var steer = Steer(velocity.normalized * data.maxSpeed);
        return steer;
    }

    private Vector2 Cohesion(IEnumerable<Fish> boids)
    {
        if (!boids.Any()) return Vector2.zero;

        var sumPositions = Vector2.zero;
        foreach (var boid in boids)
        {
            sumPositions += (Vector2)boid.transform.position;
        }

        var average = sumPositions / boids.Count();
        var direction = average - (Vector2)transform.position;
        var steer = Steer(direction.normalized * data.maxSpeed);
        return steer;
    }

    private Vector2 Separation(IEnumerable<Fish> boids)
    {
        var direction = Vector2.zero;
        boids = boids.Where(boid => Vector2.Distance(transform.position, boid.transform.position) <= data.separationRadius);
        if (!boids.Any()) return direction;

        foreach (var boid in boids)
        {
            Vector2 difference = transform.position - boid.transform.position;
            direction += difference.normalized;
        }

        direction /= boids.Count();
        var steer = Steer(direction.normalized * data.maxSpeed);
        return steer;
    }

    private Vector2 Steer(Vector2 desired)
    {
        var steer = desired - velocity;
        steer = LimitMagnitude(steer, data.maxForce);
        return steer;
    }

    private Vector2 LimitMagnitude(Vector2 baseVector, float maxMagnitude)
    {
        if (baseVector.sqrMagnitude > maxMagnitude * maxMagnitude)
        {
            baseVector = baseVector.normalized * maxMagnitude;
        }

        return baseVector;
    }

    private void OnDrawGizmosSelected()
    {
        // Neighborhood radius.
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, data.neighborhoodRadius);

        // Separation radius.
        Gizmos.color = Color.salmon;
        Gizmos.DrawWireSphere(transform.position, data.separationRadius);
    }
}