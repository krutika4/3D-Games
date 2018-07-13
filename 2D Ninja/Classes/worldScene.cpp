#include "worldScene.h"
#include <iostream>
#include "SimpleAudioEngine.h"

class ScaledSpriteCompare {
public:
  bool operator()(const ScaledSprite* lhs, const ScaledSprite* rhs) {
    return lhs->getScale() < rhs->getScale();
  }
};

World::World() :
  painterSprites(),
  numberOfSprites(40),
  maxX(400),
  maxY(300),
  minX(50),
  minY(50),
  minVelocityX(102),
  maxVelocityX(160),
  minVelocityY(102),
  maxVelocityY(160)
{
  int i = numberOfSprites;
  while (i > 0)
  {
    ScaledSprite* scaledSprite = new ScaledSprite(cocos2d::Vec2(rand() % maxVelocityX + minVelocityX, rand() % maxVelocityY + minVelocityY), cocos2d::Vec2(rand() % maxX + minX, rand() % maxY + minY), "lantern.png", 0.2f, 0.8f);
    painterSprites.push_back(scaledSprite);
    i--;
  }
  painterSprites.sort(ScaledSpriteCompare());
}

World::~World()
{
  std::list<ScaledSprite*>::iterator iter = painterSprites.begin();
  while (iter != painterSprites.end()) {
    delete (*iter);
    iter = painterSprites.erase(iter);
  }

}

cocos2d::Scene* World::createScene() {
  cocos2d::Scene* scene = cocos2d::Scene::create();
  cocos2d::Layer* layer = World::create();
  scene->addChild(layer);
  return scene;
}

bool World::init() {
  if (!Layer::init()) return false;

  visibleSize = cocos2d::Director::getInstance()->getVisibleSize();
  origin = cocos2d::Director::getInstance()->getVisibleOrigin();

  cocos2d::Sprite* bg1 = cocos2d::Sprite::create("bg1.png");

  // position the sprite on the center of the screen
  bg1->setPosition(cocos2d::Vec2(visibleSize.width / 2 + origin.x,
    visibleSize.height / 2 + origin.y));

  // add the sprite as a child to this layer
  addChild(bg1, -2);

  cocos2d::Sprite* bg2 = cocos2d::Sprite::create("bg2.png");

  // position the sprite on the center of the screen
  bg2->setPosition(cocos2d::Vec2(visibleSize.width / 2 + origin.x,
    visibleSize.height / 2 + origin.y));

  // add the sprite as a child to this layer
  addChild(bg2, -2);

  unsigned int i = 0;
  std::list<ScaledSprite*>::iterator ptr = painterSprites.begin();
  while (ptr != painterSprites.end())
  {
    if(i<20)
      bg1->addChild((*ptr)->getSprite());
    else
      bg2->addChild((*ptr)->getSprite());
    ++ptr;
    ++i;
  }
  addChild(foreground = Foreground::create(), 1);
  addChild(centerground = Centerground::create(), 0);
  addChild(background = Background::create(), -1);

  scheduleUpdate();
  CocosDenshion::SimpleAudioEngine::getInstance()->playBackgroundMusic("Surreal-Chase.mp3", true);
  return true;
}

void World::update(float dt) {

  std::list<ScaledSprite*>::iterator ptr = painterSprites.begin();
  while(ptr!=painterSprites.end())
  {
    (*ptr)->update(dt);
    ++ptr;
  }

  foreground->update(dt);
  centerground->update(dt);
  background->update(dt);
}



