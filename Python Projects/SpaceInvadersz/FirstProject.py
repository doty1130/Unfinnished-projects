import pygame
import random
import math

#initialize the pygame
pygame.init()

# create the screen
screen = pygame.display.set_mode((800, 600))

#score 
score = 0

#Title and Icon
pygame.display.set_caption("Space Invaders")
icon = pygame.image.load('./assets/Icons/favicon-32x32.png')
pygame.display.set_icon(icon)

#Game Wallpaper.
Wallpaper = pygame.image.load('./assets/Background/Wallpaper1.jpg')
Wallpaper = pygame.transform.scale(Wallpaper,(800,600))

#bullet Image
#ready - you cant see the bullet on the screen
#Fire - The bullet is moving.
BulletI = pygame.image.load('./assets/Ammo/Bullet.png')
BulletI = pygame.transform.scale(BulletI,(10,10))
BulletX = 370
BulletY = 480
BulletSx = 0 
BulletSy = 1
Bullet_State = "Ready"

#player
PlayerI =  pygame.image.load('./assets/GameCharaters/player.png')
PlayerI =  pygame.transform.scale(PlayerI,(80,80))
playerX =  370
playerY =  480
playerSx = 0
playerSy = 0

#Enemy
EnemyI =  pygame.image.load('./assets/GameCharaters/Enemys.png')
EnemyI =  pygame.transform.scale(EnemyI,(80,80))
EnemyX =  random.randint(50, 750)
EnemyY =  50
EnemySx = .3
EnemySy = 40


#player function, blit means to draw
def player(x, y):
    screen.blit(PlayerI, (x, y))

#enemy Function
def enemy(x,y):
    screen.blit(EnemyI, (x, y))

#Bullet Function
def bullet(x,y):  
    global Bullet_State
    Bullet_State = "Fire"
    screen.blit(BulletI, (x, y + 10))

def isCollision(x1, y1, x2, y2):
    distance = math.sqrt(math.pow(x1 - x2, 2) + math.pow(y1 - y2, 2))
    if distance < 27:
        return True
    else:
        return False
  
  
#gameloop
running = True
while running:
    
    screen.fill((255, 200, 100))
    screen.blit(Wallpaper, (0,0))
    
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            running = False
        if event.type == pygame.KEYDOWN:
            if event.key == pygame.K_LEFT:
               playerSx = -.3 
            if event.key == pygame.K_RIGHT:
               playerSx = .3
            if event.key == pygame.K_UP:
               playerSy = -.3
            if event.key == pygame.K_DOWN:
               playerSy = .3
            if event.key == pygame.K_SPACE:
                BulletX == playerX
                bullet(BulletX, BulletY)
        if event.type == pygame.KEYUP:
            if event.key == pygame.K_LEFT or event.key == pygame.K_RIGHT or event.key == pygame.K_UP or event.key == pygame.K_DOWN:
                playerSy = 0
                playerSx = 0

   

    #player Movement Math
    playerX += playerSx
    playerY += playerSy

    #Game Boundery
    if playerX <= 25:
        playerX = 25
    elif playerX >=700:
        playerX = 700
    
    if playerY <= 25:
        playerY = 25
    elif playerY >=500:
        playerY = 500
    # Enemy Movement
    if EnemyX <= 25:
        EnemySx = .3
        EnemyY += EnemySy
    elif EnemyX >=700:
        EnemySx = -.3
    # Bullet Movement
    if BulletY <= 0:
        BulletY = 480
        Bullet_State = "Ready"
    
    if Bullet_State is "Ready":
       BulletX = playerX
    
    if Bullet_State is "Fire":
        bullet(BulletX, BulletY)
        BulletY -= BulletSy
       
       
    #Collision
    
    collision = isCollision(EnemyX, EnemyY, BulletX, BulletY)
        
    if collision:
        BulletY = 480
        Bullet_State = "Ready"
        score += 1
        print(score)
        EnemyX = random.randint(0, 800)
        EnemyY = random.randint(50, 150)
        
        
    EnemyX += EnemySx    

    enemy(EnemyX, EnemyY)
    player(playerX, playerY)
    pygame.display.update()