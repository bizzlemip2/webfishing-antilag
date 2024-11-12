using GDWeave.Godot.Variants;
using GDWeave.Godot;
using GDWeave.Modding;

namespace GDWeave.AntiLagWebFishing;
public class PlayerPatcher : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

    // returns a list of tokens for the new script, with the input being the original script's tokens
    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        // wait for any newline token after any extends token
        var waiter = new MultiTokenWaiter([
            t => t is IdentifierToken {Name: "Camera"},
            t => t.Type is TokenType.Newline,
            t => t is IdentifierToken {Name: "camera"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken {Name: "queue_free"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t.Type is TokenType.ParenthesisClose,
        ], allowPartialMatch: false);
        var waiter2 = new MultiTokenWaiter([
            t => t.Type is TokenType.Newline,
            t => t is IdentifierToken {Name: "_process_sounds"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Newline,
            t => t.Type is TokenType.Newline,
            t => t.Type is TokenType.CfIf,
        ], allowPartialMatch: false);
        // loop through all tokens in the script
        foreach (var token in tokens)
        {
            if (waiter.Check(token))
            {
                waiter.Reset();
                yield return token;
                string[] objectNames = ["cam_base", "fish_catch_timer", "step_timer", "image_update", "rain_timer", "metaldetect_timer", "cosmetic_refresh", "detection_zones", "interact_range", "water_detect", "paint_node", "metaldetect_dot", "local_range", "safe_check", "raincloud_check", "camera_freecam_anchor", "catch_cam_position", "CollisionShape", "lean_help", "rot_help"];
                foreach (string objectName in objectNames)
                {
                    yield return new Token(TokenType.Newline, 1);
                    yield return new Token(TokenType.Dollar);
                    yield return new IdentifierToken(objectName);
                    yield return new Token(TokenType.Period);
                    yield return new IdentifierToken("queue_free");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new Token(TokenType.ParenthesisClose);
                }
                modInterface.Logger.Information("Player Anti-Lag Completed!");

            }
            else if (waiter2.Check(token)) {
                yield return token;
                yield return new IdentifierToken("controlled");
                yield return new Token(TokenType.OpAnd);
            }
            else
            {
                // return the original token
                yield return token;
            }
        }
    }

    public IModInterface modInterface;
    public PlayerPatcher(IModInterface pmodInterface)
    {
        modInterface = pmodInterface;
    }
}