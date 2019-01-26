using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour {
    private const int x = 5;
    private const int y = 5;
    [SerializeField] private GameObject empty;
    [SerializeField] private GameObject corridorCorner;
    [SerializeField] private GameObject corridorCross;
    [SerializeField] private GameObject corridorStraight;
    [SerializeField] private GameObject corridorT;
    [SerializeField] private GameObject room1;
    [SerializeField] private GameObject room2Corner;
    [SerializeField] private GameObject room2Straight;
    [SerializeField] private GameObject room3;
    [SerializeField] private GameObject room4;
    private GameObject[,] rooms;

    private void Start() {
        rooms = new GameObject[x, y];

        createGrid();
        criticalPath();
        nonCritical();
        populateRooms();
        debugCritical();
    }

    private void debugCritical() {
        for(int i = 0; i < x; i++) {
            for(int j = 0; j < y; j++) {
                if(rooms[i, j].GetComponent<Room>().getCritical()) {
                    Transform obj = rooms[i, j].transform.GetChild(0);
                    
                    for(int num = 0; num < obj.childCount; num++) {
                        obj.GetChild(num).GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                }
            }
        }
    }

    private void createGrid() {
        //Place initial empty gameobjects in correct spots for rooms;
        for(int i = 0; i < x; i++) {
            for(int j = 0; j < y; j++) {
                rooms[i, j] = Instantiate(empty, new Vector3(i * -7.5f, 1.75f, j * 7.5f), Quaternion.identity, transform);
                rooms[i, j].transform.name = "(" + i + ", " + j + ")" ;
            }
        }
    }

    private void criticalPath() {
        //Creating a path the player can definitely follow to traverse the house
        int i = Random.Range(0, x);
        int j = 0;

        //Start room
        rooms[i, j].GetComponent<Room>().setCritical(true);

        int chosen = -1;
       
        while(j < y) {
            Room currentRoom = rooms[i, j].GetComponent<Room>();

            if(chosen == -1) {
                //Just reached level
                float dir = Random.Range(0.0f, 1.0f);

                if(dir < 0.4f) {
                    //Go left if possible
                    if(i > 0) {
                        //Left
                        chosen = 0;
                        i--;
                        currentRoom.left = true;
                        rooms[i, j].GetComponent<Room>().right = true;
                    } else {
                        //Right
                        chosen = 1;
                        i++;
                        currentRoom.right = true;
                        rooms[i, j].GetComponent<Room>().left = true;
                    }
                } else if(dir < 0.8f) {
                    //Go right if possible
                    if(i < x - 1) {
                        //Right
                        chosen = 1;
                        i++;
                        currentRoom.right = true;
                        rooms[i, j].GetComponent<Room>().left = true;
                    } else {
                        //Left
                        chosen = 0;
                        i--;
                        currentRoom.left = true;
                        rooms[i, j].GetComponent<Room>().right = true;
                    }
                } else {
                    //Go down
                    chosen = -1;
                    j++;
                    
                    if(j < y) {
                        currentRoom.down = true;
                        rooms[i, j].GetComponent<Room>().up = true;
                    }
                }
            } else {
                float dir = Random.Range(0.0f, 1.0f);

                if(dir < 0.5f) {
                    //Continue same direction
                    if(chosen == 0) {
                        //Try left
                        if(i > 0) {
                            //Left
                            chosen = 0;
                            i--;
                            currentRoom.left = true;
                            rooms[i, j].GetComponent<Room>().right = true;
                        } else {
                            //Go down
                            chosen = -1;
                            j++;

                            if(j < y) {
                                currentRoom.down = true;
                                rooms[i, j].GetComponent<Room>().up = true;
                            }
                        }
                    } else {
                        //Try right
                        if(i < x - 1) {
                            //Right
                            chosen = 1;
                            i++;
                            currentRoom.right = true;
                            rooms[i, j].GetComponent<Room>().left = true;
                        } else {
                            //Go down
                            chosen = -1;
                            j++;

                            if(j < y) {
                                currentRoom.down = true;
                                rooms[i, j].GetComponent<Room>().up = true;
                            }
                        }
                    }
                } else {
                    //Go down
                    chosen = -1;
                    j++;

                    if(j < y) {
                        currentRoom.down = true;
                        rooms[i, j].GetComponent<Room>().up = true;
                    }
                }
            }

            if(j < y) {
                rooms[i, j].GetComponent<Room>().setCritical(true);
            }
        }
    }

    private void nonCritical() {
        bool changing = true;

        while(changing) {
            changing = false;

            for(int i = 0; i < x; i++) {
                for(int j = 0; j < y; j++) {
                    Room room = rooms[i, j].GetComponent<Room>();

                    if(!room.getCritical()) {
                        if(i > 0) {
                            if(rooms[i - 1, j].GetComponent<Room>().getAttached()) {
                                if(!room.left) {
                                    rooms[i - 1, j].GetComponent<Room>().right = true;
                                    room.setAttached(true);
                                    room.left = true;
                                    changing = true;
                                }
                            }
                        }

                        if(i < x - 1) {
                            if(rooms[i + 1, j].GetComponent<Room>().getAttached()) {
                                if(!room.right) {
                                    rooms[i + 1, j].GetComponent<Room>().left = true;
                                    room.setAttached(true);
                                    room.right = true;
                                    changing = true;
                                }
                            }
                        }

                        if(j > 0) {
                            if(rooms[i, j - 1].GetComponent<Room>().getAttached()) {
                                if(!room.up) {
                                    rooms[i, j - 1].GetComponent<Room>().down = true;
                                    room.setAttached(true);
                                    room.up = true;
                                    changing = true;
                                }
                            }
                        }

                        if(j < y - 1) {
                            if(rooms[i, j + 1].GetComponent<Room>().getAttached()) {
                                if(!room.down) {
                                    rooms[i, j + 1].GetComponent<Room>().up = true;
                                    room.setAttached(true);
                                    room.down = true;
                                    changing = true;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void populateRooms() {
        for(int i = 0; i < x; i++) {
            for(int j = 0; j < y; j++) {
                if(rooms[i, j] != null) {
                    Room room = rooms[i, j].GetComponent<Room>();
                    int num = room.getAdjoining();
                    bool left = room.left;
                    bool right = room.right;
                    bool up = room.up;
                    bool down = room.down;
                    bool opposite = (left && right) || (up && down);

                    randomRoom(rooms[i, j].transform, num, opposite);

                    switch(num) {
                        case 1:
                            if(left) {
                                room.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                            } else if(right) {
                                room.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                            } else if(up) {
                                room.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                            } else if(down) {
                                room.transform.eulerAngles = new Vector3(0.0f, 270.0f, 0.0f);
                            }

                            break;
                        case 2:
                            if(opposite) {
                                if(left)
                                    room.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                                else if(up)
                                    room.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                            } else {
                                if(left) {
                                    if(up)
                                        room.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                                    else
                                        room.transform.eulerAngles = new Vector3(0.0f, 270.0f, 0.0f);
                                } else {
                                    if(up)
                                        room.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                                    else
                                        room.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                                }
                            }

                            break;
                        case 3:
                            if(!down)
                                room.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                            else if(!left)
                                room.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                            else if(!up)
                                room.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                            else if(!right)
                                room.transform.eulerAngles = new Vector3(0.0f, 270.0f, 0.0f);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    private GameObject randomRoom(Transform parent, int num, bool opposite) {
        float rand = Random.Range(0.0f, 1.0f);

        if(rand < 0.33f) {
            //Room
            switch(num) {
                case 1:
                    return Instantiate(room1, parent);
                case 2:
                    if(opposite)
                        return Instantiate(room2Straight, parent);
                    else
                        return Instantiate(room2Corner, parent);
                case 3:
                    return Instantiate(room3, parent);
                case 4:
                    return Instantiate(room4, parent);
                default:
                    return null;
            }
        } else {
            //Corridor
            switch(num) {
                case 1:
                    //No corridor with one entrance
                    return Instantiate(room1, parent);
                case 2:
                    if(opposite)
                        return Instantiate(corridorStraight, parent);
                    else
                        return Instantiate(corridorCorner, parent);
                case 3:
                    return Instantiate(corridorT, parent);
                case 4:
                    return Instantiate(corridorCross, parent);
                default:
                    return null;
            }
        }

    }
}
