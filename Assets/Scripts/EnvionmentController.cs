using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;


public class EnvironmentController : MonoBehaviour
{
    //public List<AgentController> agents;  // Lista di tutti gli agenti
     public List<DroneAgent> agents;  // Lista di agenti nel team
    private int activeAgents;  // Riferimento agli agenti nel team

    private SimpleMultiAgentGroup agentGroup; 
    public EnemyScript enemy;  

    void Start()
    {
        // Inizializza il numero di agenti attivi
         agentGroup = new SimpleMultiAgentGroup();

        // Aggiungi ogni agente al gruppo
        foreach (var agent in agents)
        {
            agentGroup.RegisterAgent(agent);
        }

        // Inizializza il numero di agenti attivi
        activeAgents = agents.Count;
    }

    public void ResetAgents()
    {
        // Ripristina la posizione e riattiva tutti gli agenti
        foreach (var agent in agents)
        {
            agent.transform.position = agent.GetInitialPosition(); // Assumendo che tu abbia un metodo per ottenere la posizione iniziale
            agent.gameObject.SetActive(true);
        }

        // Reimposta il numero di agenti attivi
        activeAgents = agents.Count;
    }

    


    // Metodo per penalizzare il gruppo quando un agente tocca il muro o un altro agente
    public void PenalizeGroup(float penalty)
    {
        agentGroup.AddGroupReward(penalty);
    }


    // Metodo per rimuovere un agente dall'episodio
    public void RemoveAgent(DroneAgent agent)
{
    // Verifica che l'agente non sia null
    if (agent != null)
    {
        // Riduci il conteggio degli agenti attivi
        activeAgents--;

        // Disattiva l'agente
        agent.gameObject.SetActive(false);

        // Controlla se tutti gli agenti sono stati eliminati
        if (activeAgents <= 0)
        {
            agentGroup.EndGroupEpisode();
            ResetAgents();
        }
    }
    else
    {
        Debug.LogError("L'agente passato a RemoveAgent è null.");
    }
}

    // Metodo per resettare l'episodio


    public void EnemyReachedCenter()
    {
        enemy.ResetPosition();
        Debug.Log("Nemico ha raggiunto target");
        
        agentGroup.AddGroupReward(-2.0f);
        agentGroup.EndGroupEpisode();
         ResetAgents();
        
    }

    

    // Metodo chiamato quando un agente tocca il nemico
    public void EnemyStopped()
    {
        enemy.ResetPosition();
        Debug.Log("Nemico Fermato");
        
        agentGroup.AddGroupReward(2.0f);
        agentGroup.EndGroupEpisode();
         ResetAgents();
    }

    // Metodo chiamato quando un agente tocca un muro
    public void AgentHitWall(DroneAgent agent)
    {
        enemy.ResetPosition();
        Debug.Log("Muro");

       agentGroup.AddGroupReward(-0.3f);
        
    }


    public void AgentFloor(DroneAgent agent)
    {
        enemy.ResetPosition();
        Debug.Log("Floor");

       agentGroup.AddGroupReward(-0.6f);
        
    }


     public void AgentHitTarget()
    {
        enemy.ResetPosition();
        Debug.Log("target colpito da agente");

        agentGroup.AddGroupReward(-1.0f);
        agentGroup.EndGroupEpisode();
         ResetAgents();
    }

    
}
