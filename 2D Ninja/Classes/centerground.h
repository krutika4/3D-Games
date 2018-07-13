#include "cocos2d.h"
#include "utilities.h"
#include "scaledSprite.h"
#include <vector>

class Centerground : public cocos2d::Layer {

public:
  Centerground();
  void update(float dt);
  virtual bool init();
  CREATE_FUNC(Centerground);
  
private:
  cocos2d::Sprite* bg1;
  cocos2d::Sprite* bg2;
  cocos2d::Sprite* bg3;
  int scrollSpeed = 1;
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
