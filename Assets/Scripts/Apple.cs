using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player {
    NONE,
    PLAYER1,
    PLAYER2
}

public class Apple : MonoBehaviour
{
    private int number;
    private Player owner = Player.NONE;

    public int Number { get { return number; } }
    public Player Owner { get { return owner; } }

    public void Setting(int number, Player owner, Color color) {
        this.number = number;
        this.owner = owner;
        GetComponent<SpriteRenderer>().color = color;
    }
}
