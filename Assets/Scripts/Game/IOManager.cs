/// Bridge that connects the game logic and the unity simulation
public class IOManager 
{

    private Game game;

    private Trajectory pendingData;

    public void SetGame(Game game) 
    {
        this.game = game;
    }

    /// Triggered by Unity
    /// TODO: Connecter l'évènement du clic du bouton "Start" à cette méthode (sur Unity)
    public void onStartClicked() 
    {
        if(game == null) return;

        game.StartGame();
    }

    /// Triggered by Unity
    /// TODO: Connecter l'évènement du clic du bouton "Continuer le jeu après un lancer" à cette méthode (sur Unity)
    public void onNextClicked() 
    {
        if(game == null) return;

        game.MarkReadyForThrow();
    }

    /// Triggered by Game
    public void OnGameStarted() 
    {
        // TODO: Soit ne rien faire, soit éventuellement afficher un message temporaire "La partie commence" ou quoi
    }

    /// Triggered by Game
    public void OnGameEnded() 
    {
        // TODO: Afficher un message du style "Fin de la partie (ou fin de la manche je sais pas encore)" à l'écran
    }

    /// Triggered by Unity
    /// TODO: Connecter l'évènement "Le robot passe la ligne de départ et donc est considéré comme lancé" à cette méthode (dans le code lié à Unity probablement)
    public void OnStoneThrown(Trajectory traj) 
    {
        if(game == null) return;

        if(game.ExpectsThrow()) // if this returns false, it means that the stone was most likely thrown by accident
        {
            this.pendingData = traj;
            // TODO: Lancer la simulation visuelle et robotique
        }
    }

    /// Triggered by Unity / The Simulation manager
    /// TODO: Connecter l'évènement "La simulation du lancer est terminée" à cette méthode 
    public void OnThrowSimulationEnded()
    {
        if(game == null || pendingData == null) return; // pendingData should normally never be null at this stage

        game.PlayThrow(pendingData);
        pendingData = null;

        // TODO: Peut-être afficher un message genre "Bon lancer !" ou un truc du style ?
    }

    /// Triggered by Game
    public void ShowScore(int score, int scorer)
    {
        // TODO: Actualiser le score en haut de l'interface sur Unity
    }

    /// Triggered by Game
    public void AllowNext()
    {
        // TODO: Réactiver le bouton "Continuer le jeu après un lancer"
    }

}