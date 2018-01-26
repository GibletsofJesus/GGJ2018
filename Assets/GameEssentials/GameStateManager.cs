using UnityEngine;
namespace State
{
    public abstract class GameStateManager : MonoBehaviour
    {
        protected GameState p_state;
        protected GameState p_prevState;
        protected float p_gameDelta = 0;
        protected float p_gameTime = 0;
        protected GameStateManager p_instance = null;
        public GameState gameState { get { return p_state; } }
        public float gameDeltaTime { get { return p_gameDelta; } }
        public float gameTime { get { return p_gameTime; } }
        public GameStateManager instance { get { return p_instance; } }

        protected virtual void Awake()
        {
            if (p_instance == null)
            {
                p_instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public virtual void ChangeState(GameState _toChange)
        {
            p_prevState = p_state;
            p_state = _toChange;
            StateSwitch();
        }

        protected virtual void StateSwitch()
        {
            switch (p_state)
            {
                case GameState.GAME_OVER:
                    GameOver();
                    break;
                case GameState.MENU:
                    Menu();
                    break;
                case GameState.PAUSE:
                    Pause();
                    break;
                case GameState.PLAY:
                    Play();
                    break;
                case GameState.REVERSE:
                    Reverse();
                    break;
                case GameState.TITLE:
                    Title();
                    break;
            }

        }

        protected virtual void GameOver() { }
        protected virtual void Play()
        {
            p_gameDelta = Time.deltaTime;
            p_gameTime += Time.deltaTime;
        }
        protected virtual void Pause() { }
        protected virtual void Reverse() { p_gameDelta = -Time.deltaTime; }
        protected virtual void Menu() { }
        protected virtual void Title() { }


        protected virtual void Update()
        {
            StateSwitch();
        }

        protected void ResetTime()
        {
            p_gameDelta = 0;
            p_gameTime = 0;
        }
    }


    public enum GameState
    {
        PLAY,
        PAUSE,
        REVERSE,
        MENU,
        TITLE,
        GAME_OVER,
    }
}