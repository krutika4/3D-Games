#include "background.h"
#include <iostream>
#include "SimpleAudioEngine.h"

Background::Background() :
  bg1(cocos2d::Sprite::create("bg3.png")),
  bg2(cocos2d::Sprite::create("bg3.png")),
  bg3(cocos2d::Sprite::create("bg3.png")),
  playerVelocityX(0),
  playerX(100),
  playerY(50),
  player(new MultiSprite(cocos2d::Vec2(playerVelocityX, 0), cocos2d::Vec2(playerX, playerY),
    "ninja.plist", "Ninja%1d.png", 8)),
  bulletList(),
  freeList(),
  numberOfSprites(20),
  sprites(),
  score(0),
  scoreLabel(cocos2d::Label::createWithTTF("score: 0",
    "fonts/arial.ttf", 12)),
  freeListCount(cocos2d::Label::createWithTTF("freeList count: 0",
    "fonts/arial.ttf", 12)),
  bulletListCount(cocos2d::Label::createWithTTF("bulletList count: 0",
    "fonts/arial.ttf", 12)),
  instruction(cocos2d::Label::createWithTTF("Shooting: Mouse click or press S",
    "fonts/arial.ttf", 9))
{
  bg1->setPosition(0, visibleSize.height / 2);
  bg2->setPosition(bg1->boundingBox().size.width - 1, visibleSize.height / 2);
  bg3->setPosition(bg1->boundingBox().size.width + bg2->boundingBox().size.width - 1, visibleSize.height / 2);

  int i = 0;
  while (i < numberOfSprites)
  {
    ExplodableSprite* explodableSprite = new ExplodableSprite(cocos2d::Vec2(-102, 0), cocos2d::Vec2((i + 1) * 500, 50), "enemy.png");
    sprites.push_back(explodableSprite);
    i++;
  }
}

Background::~Background()
{
  delete player;
  std::list<BasicSprite*>::iterator iterBulletList = bulletList.begin();
  while (iterBulletList != bulletList.end()) {
    delete (*iterBulletList);
    iterBulletList = bulletList.erase(iterBulletList);
  }
  std::list<BasicSprite*>::iterator iterFreeList = freeList.begin();
  while (iterFreeList != freeList.end()) {
    delete (*iterFreeList);
    iterFreeList = freeList.erase(iterFreeList);
  }
  std::list<Drawable*>::iterator iter = sprites.begin();
  while (iter != sprites.end()) {
    delete (*iter);
    iter = sprites.erase(iter);
  }
}

bool Background::init() {
  if (!Layer::init()) return false;
  
  //Touch implementation
  cocos2d::EventListenerTouchOneByOne *
    listener = cocos2d::EventListenerTouchOneByOne::create();
  listener->setSwallowTouches(true);
  listener->onTouchBegan =
    CC_CALLBACK_2(Background::onTouchBegan, this);
  listener->onTouchMoved =
    CC_CALLBACK_2(Background::onTouchMoved, this);
  listener->onTouchEnded =
    CC_CALLBACK_2(Background::onTouchEnded, this);
  listener->onTouchCancelled =
    CC_CALLBACK_2(Background::onTouchEnded, this);
  listener->onTouchCancelled =
    CC_CALLBACK_2(Background::onTouchCancelled, this);
  
  //Keyboard event implementation  
  cocos2d::EventListenerKeyboard* l = cocos2d::EventListenerKeyboard::create();
  l->onKeyPressed =
    [this](cocos2d::EventKeyboard::KeyCode keyCode,cocos2d::Event* event) {
    switch(keyCode) {
	    case cocos2d::EventKeyboard::KeyCode::KEY_S:
	      Background::shoot();
	      break;
    }
   };
   
   l->onKeyReleased =
    [this](cocos2d::EventKeyboard::KeyCode keyCode, cocos2d::Event* event){
    switch(keyCode) {
	  case cocos2d::EventKeyboard::KeyCode::KEY_S:
	      CocosDenshion::SimpleAudioEngine::getInstance()->playEffect("shot.mp3");
	  break;
    }
  };
    
   cocos2d::Director::getInstance()->getEventDispatcher()->
    addEventListenerWithSceneGraphPriority(listener, this);
    
    cocos2d::Director::getInstance()->getEventDispatcher()->
    addEventListenerWithSceneGraphPriority(l, this);
   
  addChild(stretchFit(bg1));
  addChild(stretchFit(bg2));
  addChild(stretchFit(bg3));
  addChild(player->getSprite());
  
  std::list<Drawable*>::iterator ptrEnemy = sprites.begin();
  while (ptrEnemy != sprites.end())
  {
    addChild((*ptrEnemy)->getSprite());
    ++ptrEnemy;
  }

  scoreLabel->setPosition((visibleSize.width*8) / 9,  (visibleSize.height*8)/9);
  scoreLabel->setTextColor(cocos2d::Color4B::RED);
  addChild(scoreLabel);
  
  freeListCount->setPosition((visibleSize.width * 8) / 9, (visibleSize.height * 7) / 9);
  freeListCount->setTextColor(cocos2d::Color4B::RED);
  addChild(freeListCount);
  
  bulletListCount->setPosition((visibleSize.width * 8) / 9, (visibleSize.height * 6) / 9);
  bulletListCount->setTextColor(cocos2d::Color4B::RED);
  addChild(bulletListCount);

  instruction->setPosition((visibleSize.width*3) / 9, (visibleSize.height * 8) / 9);
  instruction->setTextColor(cocos2d::Color4B::RED);
  addChild(instruction);

  return true;
}
void Background::update(float dt) {
  srand(time(NULL));
  freeListCount->setString("Free List Count: " + std::to_string(freeList.size()));
  bulletListCount->setString("Bullet List Count: " + std::to_string(bulletList.size()));

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
  
  //Start - Update Bullets
  std::list<BasicSprite*>::iterator ptr = bulletList.begin();
  while (ptr != bulletList.end())
  {
    if (!(*ptr)->getIsRendered())
    {
      addChild((*ptr)->getSprite());
      (*ptr)->getSprite()->release();
      (*ptr)->setIsRendered(true);
    }
    (*ptr)->update(dt);
    std::list<Drawable*>::iterator ptrEnemy = sprites.begin();
    while (ptrEnemy != sprites.end())
    {
      //-
      if ((*ptr)->collidedRect(*ptrEnemy)) {
        if ((*ptrEnemy)->getIsExplodable() && !(*ptrEnemy)->getIsRemovable())
        {
          score = score + 10;
          scoreLabel->setString("Score: " + std::to_string(score));
          (*ptrEnemy)->makeChunks(sprites);
          (*ptrEnemy)->setIsRemovable(true);
        }
        break;
      }
      ++ptrEnemy;
      //--
    }
    if ((*ptr)->getIsRemovable())
    {
      (*ptr)->getSprite()->retain();
      removeChild((*ptr)->getSprite());
      freeList.push_back((*ptr));
      ptr = bulletList.erase(ptr);
    }
    else
      ++ptr;
  }  //End - Update Bullets
  
  //Start - Update enemy sprites
  std::list<Drawable*>::iterator ptrEnemy = sprites.begin();
  while (ptrEnemy != sprites.end())
  {
    if (!(*ptrEnemy)->getIsRendered())
    {
      addChild((*ptrEnemy)->getSprite());
      (*ptrEnemy)->setIsRendered(true);
    }
    (*ptrEnemy)->update(dt);
    if ((*ptrEnemy)->getIsRemovable())
    {
      removeChild((*ptrEnemy)->getSprite());
      delete(*ptrEnemy);
      ptrEnemy = sprites.erase(ptrEnemy);
    }
    else
      ++ptrEnemy;
  }
  //End - Update enemy sprites
}

void Background::shoot(){
	freeListCount->setString("Free List Count: " + std::to_string(freeList.size()));
  bulletListCount->setString("Bullet List Count: " + std::to_string(bulletList.size()));

  if (freeList.empty())
  {
    BasicSprite* newbullet = new BasicSprite(cocos2d::Vec2(300, 0), cocos2d::Vec2((player->getSprite())->getPosition().x, (player->getSprite())->getPosition().y), "laser.png");
    bulletList.push_back(newbullet);
    (newbullet->getSprite())->retain();
  }
  else
  {
    BasicSprite* bullet = freeList.front();
    freeList.pop_front();
    bulletList.push_back(bullet);
    bullet->reset(cocos2d::Vec2((player->getSprite())->getPosition().x, (player->getSprite())->getPosition().y));
  }
}

bool Background::onTouchBegan(cocos2d::Touch *touch, cocos2d::Event *event) {
  Background::shoot();
  return true;
}
void Background::onTouchMoved(cocos2d::Touch *touch, cocos2d::Event *event) {}

void Background::onTouchEnded(cocos2d::Touch *touch, cocos2d::Event *event) {
	CocosDenshion::SimpleAudioEngine::getInstance()->playEffect("shot.mp3");
}

void Background::onTouchCancelled(cocos2d::Touch *touch, cocos2d::Event *event) {
	CocosDenshion::SimpleAudioEngine::getInstance()->playEffect("shot.mp3");
}
