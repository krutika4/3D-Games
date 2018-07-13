#ifndef __WORLD_SCENE_H__
#define __WORLD_SCENE_H__

#include "cocos2d.h"
#include "utilities.h"
#include <list>
#include "foreground.h"
#include "centerground.h"
#include "background.h"
#include "scaledSprite.h"

class World : public cocos2d::Layer
{
public:
  World();
  ~World();
  void update(float dt);
  static cocos2d::Scene* createScene();
  virtual bool init();
  CREATE_FUNC(World);
  
  
private:
  Foreground*   foreground;
  Centerground* centerground;
  Background*   background;
  std::list<ScaledSprite*> painterSprites;
  int numberOfSprites;
  int maxX;
  int maxY;
  int minX;
  int minY;
  int minVelocityX;
  int maxVelocityX;
  int minVelocityY;
  int maxVelocityY;
};

#endif 



