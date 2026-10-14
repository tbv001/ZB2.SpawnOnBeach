using UnityEngine;

namespace SpawnOnBeach.Classes;

public static class Helper
{
    public static float GetClosestDanger(Vector3 position)
    {
        var minSqrDist = float.PositiveInfinity;

        // Compare distances from all players
        var playersCtrl = PlayersController.instance;
        if (playersCtrl != null && playersCtrl.players != null)
        {
            foreach (var player in playersCtrl.players)
            {
                if (player != null)
                {
                    var sqrDist = (player.transform.position - position).sqrMagnitude;
                    if (sqrDist < minSqrDist)
                    {
                        minSqrDist = sqrDist;
                    }
                }
            }
        }

        // Compare distances from all zombies
        var zombieLoader = ZombieLoader.Instance;
        if (zombieLoader != null)
        {
            if (zombieLoader.zombies != null)
            {
                foreach (var zombie in zombieLoader.zombies)
                {
                    if (zombie?.obj == null)
                        continue;

                    var sqrDist = (zombie.obj.transform.position - position).sqrMagnitude;
                    if (sqrDist < minSqrDist)
                    {
                        minSqrDist = sqrDist;
                    }
                }
            }

            if (zombieLoader.zombieProps != null)
            {
                foreach (var prop in zombieLoader.zombieProps)
                {
                    if (prop != null)
                    {
                        var sqrDist = (prop.transform.position - position).sqrMagnitude;
                        if (sqrDist < minSqrDist)
                        {
                            minSqrDist = sqrDist;
                        }
                    }
                }
            }

            if (zombieLoader.unloadedZombies != null)
            {
                foreach (var unloaded in zombieLoader.unloadedZombies)
                {
                    if (unloaded != null)
                    {
                        var sqrDist = (unloaded.transform.position - position).sqrMagnitude;
                        if (sqrDist < minSqrDist)
                        {
                            minSqrDist = sqrDist;
                        }
                    }
                }
            }
        }

        return minSqrDist;
    }
}
