#include "basicSprite.h"
#include "drawable.h"
#include <iostream>
BasicSprite::BasicSprite(const cocos2d::Vec2& v, const cocos2d::Vec2& p, const char* imgFile) :
  Drawable(v, p),
  sprite(cocos2d::Sprite::create(imgFile)),
  isRemovable(false),
  lifetime(0.0f),
  isRendered(false)
{
  sprite->setPosition(p.x, p.y);
}

BasicSprite::BasicSprite(const BasicSprite& e) :
  Drawable(e),
  sprite(e.sprite)
{

}

BasicSprite::~BasicSprite()
{
}


void BasicSprite::update(float dt)
{
  lifetime += dt;
  cocos2d::Point position = sprite->getPosition();
  cocos2d::Vec2 incr = getVelocity() * dt;
  sprite->setPosition(position.x + incr.x, position.y + incr.y);
  if (lifetime > 1.0f)
    isRemovable = true;
}

void BasicSprite::reset(const cocos2d::Vec2& vec)
{
  setIsRemovable(false);
  (getSprite())->setPosition(cocos2d::Vec2(vec.x, vec.y));
  lifetime = 0.0f;
  isRendered = false;
}

bool BasicSprite::collidedRect(const Drawable* otherSprite) const {
  int myWidth = getSprite()->getContentSize().width;
  int myHeight = getSprite()->getContentSize().height;
  int oWidth = otherSprite->getSprite()->getContentSize().width;
  int oHeight = otherSprite->getSprite()->getContentSize().height;

  cocos2d::Point myPos = getSprite()->getPosition();
  cocos2d::Point oPos = otherSprite->getSprite()->getPosition();

  if (myPos.x + myWidth / 2 < oPos.x - oWidth / 2) return false;
  if (myPos.x - myWidth / 2 > oPos.x + oWidth / 2) return false;
  if (myPos.y - myHeight / 2 > oPos.y + oHeight / 2) return false;
  if (myPos.y + myHeight / 2 < oPos.y - oHeight / 2) return false;
  return true;
}

