using Godot;
using System;

public partial class Unit : CharacterBody2D
{
    [Export] private int health = 100;
    [Export] private int damage = 20;
    [Export] private float moveSpeed = 50.0f;
    [Export] private float attackRange = 20;
    [Export] private float attackRate = 0.5f;
    private float lastAttackTime;
    private Unit target;
    private NavigationAgent2D agent;
    private Sprite2D sprite;
    [Export] private bool isPlayer;

    public override void _Ready()
    {
        agent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        sprite = GetNode< Sprite2D>("Sprite");
    }
    public override void _PhysicsProcess(double delta)
    {
        if (agent.IsNavigationFinished())
        {
            return;
        }

        Vector2 direction = GlobalPosition.DirectionTo(agent.GetNextPathPosition());
        Velocity = direction * moveSpeed;

        MoveAndSlide();
    }
    public override void _Process(double delta)
    {
        TargetCheck();
    }

    public void TargetCheck()
    {
        if (target != null)
        {
            float dist = GlobalPosition.DistanceTo(target.GlobalPosition);
            if (dist <= attackRange)
            {
                agent.TargetPosition = GlobalPosition;
                TryAttackTarget();
            }
            else
            {
                agent.TargetPosition = target.GlobalPosition;
            }
        }
    }

    public void MoveToLocation(Vector2 location)
    {
        target = null;
        agent.TargetPosition = location;
    }

    public void SetTarget(Unit target)
    {
        this.target = target;
    }

    public void TakeDamage(int damageToTake)
    {
        health -= damageToTake;

        if (health <= 0)
        {
            QueueFree();
        }
    }

    public void TryAttackTarget()
    {
        double currentTime = Time.GetUnixTimeFromSystem();
        if (currentTime > lastAttackTime)
        {
            target.TakeDamage(damage);
            lastAttackTime = (float)currentTime;
        }
    }
}
