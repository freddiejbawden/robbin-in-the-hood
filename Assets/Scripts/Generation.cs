using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour {
    private const int x = 4;
    private const int y = 4;
    [SerializeField] private GameObject[] setRooms;
    private GameObject[,] rooms;

    private void Start() {
        rooms = new GameObject[x, y];

        updateRooms();
        criticalPath();
        createRooms();
    }

    private void createRooms() {
        for(int i = 0; i < x; i++) {
            for(int j = 0; j < y; j++) {
                Room room = rooms[i, j].GetComponent<Room>();

                int adjoining = 0;

                if(room.left)
                    adjoining++;

                if(room.right)
                    adjoining++;

                if(room.up)
                    adjoining++;

                if(room.down)
                    adjoining++;

                switch(adjoining) {
                    case 0:
                        room.setColour(Color.black);
                        break;
                    case 1:
                        room.setColour(Color.cyan);
                        break;
                    case 2:
                        room.setColour(Color.green);
                        break;
                    case 3:
                        room.setColour(Color.yellow);
                        break;
                    case 4:
                        room.setColour(Color.red);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void criticalPath() {
        int xx = Random.Range(0, x);
        int yy = 0;

        rooms[xx, yy].GetComponent<Room>().setCritical(true);

        int chosen = -1;
       
        while(yy < y) {
            Room currentRoom = rooms[xx, yy].GetComponent<Room>();

            if(chosen == -1) {
                //Just reached level
                float dir = Random.Range(0.0f, 1.0f);

                if(dir < 0.4f) {
                    //Go left if possible
                    if(xx > 0) {
                        //Left
                        chosen = 0;
                        xx--;
                        currentRoom.left = true;
                        rooms[xx, yy].GetComponent<Room>().right = true;
                    } else {
                        //Right
                        chosen = 1;
                        xx++;
                        currentRoom.right = true;
                        rooms[xx, yy].GetComponent<Room>().left = true;
                    }
                } else if(dir < 0.8f) {
                    //Go right if possible
                    if(xx < x - 1) {
                        //Right
                        chosen = 1;
                        xx++;
                        currentRoom.right = true;
                        rooms[xx, yy].GetComponent<Room>().left = true;
                    } else {
                        //Left
                        chosen = 0;
                        xx--;
                        currentRoom.left = true;
                        rooms[xx, yy].GetComponent<Room>().right = true;
                    }
                } else {
                    //Go down
                    chosen = -1;
                    yy++;
                    
                    if(yy < y) {
                        currentRoom.down = true;
                        rooms[xx, yy].GetComponent<Room>().up = true;
                    }
                }
            } else {
                float dir = Random.Range(0.0f, 1.0f);

                if(dir < 0.5f) {
                    //Continue same direction
                    if(chosen == 0) {
                        //Try left
                        if(xx > 0) {
                            //Left
                            chosen = 0;
                            xx--;
                            currentRoom.left = true;
                            rooms[xx, yy].GetComponent<Room>().right = true;
                        } else {
                            //Go down
                            chosen = -1;
                            yy++;

                            if(yy < y) {
                                currentRoom.down = true;
                                rooms[xx, yy].GetComponent<Room>().up = true;
                            }
                        }
                    } else {
                        //Try right
                        if(xx < x - 1) {
                            //Right
                            chosen = 1;
                            xx++;
                            currentRoom.right = true;
                            rooms[xx, yy].GetComponent<Room>().left = true;
                        } else {
                            //Go down
                            chosen = -1;
                            yy++;

                            if(yy < y) {
                                currentRoom.down = true;
                                rooms[xx, yy].GetComponent<Room>().up = true;
                            }
                        }
                    }
                } else {
                    //Go down
                    chosen = -1;
                    yy++;

                    if(yy < y) {
                        currentRoom.down = true;
                        rooms[xx, yy].GetComponent<Room>().up = true;
                    }
                }
            }

            if(yy < y) {
                rooms[xx, yy].GetComponent<Room>().setCritical(true);
            }
        }
    }

    private void updateRooms() {
        for(int i = 0; i < x; i++) {
            for(int j = 0; j < y; j++) {
                rooms[i, j] = setRooms[i + (4 * j)];
            }
        }
    }
}
