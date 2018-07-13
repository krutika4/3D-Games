#include "cocos2d.h"
#include "utilities.h"
#include "multiSprite.h"
#include "explodableSprite.h"
#include "basicSprite.h"
#include <time.h>
#include <stdlib.h>
#include <list>
class Background : public cocos2d::Layer {
public:
  Background();
  ~Background();
  void update(float dt);
  virtual bool init();
  CREATE_FUNC(Background);
  bool onTouchBegan(cocos2d::Touch *touch, cocos2d::Event *event);
  void onTouchMoved(cocos2d::Touch *touch, cocos2d::Event *event);
  void onTouchEnded(cocos2d::Touch *touch, cocos2d::Event *event);
  void onTouchCancelled(cocos2d::Touch *touch, cocos2d::Event *event);
  void shoot();
private:
  cocos2d::Sprite* bg1;
  cocos2d::Sprite* bg2;
  cocos2d::Sprite* bg3;
  int scrollSpeed = 2;
  int playerVelocityX;
  int playerX;
  int playerY;
  MultiSprite * player;
  std::list<BasicSprite*> bulletList;
  std::list<BasicSprite*> freeList;
  int numberOfSprites;
  std::list<Drawable*> sprites;
  int score;
  cocos2d::Label* scoreLabel;
  cocos2d::Label* freeListCount;
  cocos2d::Label* bulletListCount;
  cocos2d::Label* instruction;
};

