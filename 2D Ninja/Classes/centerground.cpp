#include <cmath>
#include "centerground.h"
#include <time.h>
#include <stdlib.h>
#include <SimpleAudioEngine.h>

class ScaledSpriteCompare {
public:
  bool operator()(const ScaledSprite* lhs, const ScaledSprite* rhs) {
    return lhs->getScale() < rhs->getScale();
  }
};

Centerground::Centerground():
  bg1(cocos2d::Sprite::create("bg4.png")),
  bg2(cocos2d::Sprite::create("bg4.png")),
  bg3(cocos2d::Sprite::create("bg4.png")),
  painterSprites(),
  numberOfSprites(20),
  maxX(400),
  maxY(300),
  minX(50),
  minY(50),
  minVelocityX(102),
  maxVelocityX(160),
  minVelocityY(102),
  maxVelocityY(160)
  {
    bg1->setPosition(0, visibleSize.height / 2);
    bg2->setPosition(bg1->boundingBox().size.width - 1, visibleSize.height / 2);
    bg3->setPosition(bg1->boundingBox().size.width + bg2->boundingBox().size.width - 1, visibleSize.height / 2);

    int i = numberOfSprites;
    while (i > 0)
    {
      ScaledSprite* scaledSprite = new ScaledSprite(cocos2d::Vec2(rand() % maxVelocityX + minVelocityX, rand() % maxVelocityY + minVelocityY), cocos2d::Vec2(rand() % maxX + minX, rand() % maxY + minY), "lantern.png", 0.8f,1.0f);
      painterSprites.push_back(scaledSprite);
      i--;
    }
    painterSprites.sort(ScaledSpriteCompare());
  }

	
bool Centerground::init() {

  if ( !Layer::init() ) return false;   
  srand(time(NULL));
  addChild(stretchFit(bg1));
  addChild(stretchFit(bg2));
  addChild(stretchFit(bg3));
  std::list<ScaledSprite*>::iterator ptr = painterSprites.begin();
  while (ptr != painterSprites.end())
  {
    addChild((*ptr)->getSprite());
    ++ptr;
  }
  return true;
}

void Centerground::update(float dt) {

  bg1->setPositionX(bg1->getPositionX() - scrollSpeed);
  bg2->setPositionX(bg2->getPositionX() - scrollSpeed);
  bg3->setPositionX(bg3->getPositionX() - scrollSpeed);
  if (bg1->getPositionX() < -bg1->boundingBox().size.width / 10) {
    bg3->setPositionX(bg2->getPositionX() - scrollSpeed + (bg2->boundingBox().size.width - scrollSpeed));
  }

  if (bg2->getPositionX() < -bg2->boundingBox().size.width / 10) {
    bg1->setPositionX(bg3->getPositionX() - scrollSpeed + (bg3->boundingBox().size.width - scrollSpeed));
  }

  if (bg3->getPositionX() < -bg3->boundingBox().size.width / 10) {
    bg2->setPositionX(bg1->getPositionX() - scrollSpeed + (bg1->boundingBox().size.width - scrollSpeed));
  }

  std::list<ScaledSprite*>::iterator ptr = painterSprites.begin();
  while (ptr != painterSprites.end())
  {
    (*ptr)->update(dt);
    ++ptr;
  }
}
