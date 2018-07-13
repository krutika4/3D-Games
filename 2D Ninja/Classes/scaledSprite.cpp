#include <iostream>
#include <cmath>
#include "scaledSprite.h"
ScaledSprite::ScaledSprite(const cocos2d::Vec2& v, const cocos2d::Vec2& p, const char* imgFile, float minScale, float maxScale) :
  Drawable(v, p),
  sprite(cocos2d::Sprite::create(imgFile)),
  scale(getRandFloat(minScale, maxScale)),
  maxX(400),
  minX(100),
  maxY(300),
  minY(100)
{
  sprite->setPosition(p.x, p.y);
  sprite->setScaleX(scale);
  sprite->setScaleY(scale);
}

ScaledSprite::ScaledSprite(const ScaledSprite& s) :
  Drawable(s),
  sprite(s.sprite),
  scale(s.scale),
  maxX(s.maxX),
  minX(s.minX),
  maxY(s.maxY),
  minY(s.minY)
{ }

ScaledSprite::~ScaledSprite() {
}

void ScaledSprite::update(float dt)
{
  cocos2d::Point position = sprite->getPosition();
  cocos2d::Vec2 incr = getVelocity() * dt;
  sprite->setPosition(position.x + incr.x, position.y + incr.y);

  cocos2d::Point location = sprite->getPosition();
  if (location.x <= minX || location.x >= maxX) {
    setVelocity(cocos2d::Vec2(-1 * getVelocity().x, getVelocity().y));
  }
  if (location.y <= minY) {
    if (getVelocity().y<0)
      setVelocity(cocos2d::Vec2(getVelocity().x, -1 * getVelocity().y));
  }
  if (location.y >= maxY) {
    if (getVelocity().y>0)
      setVelocity(cocos2d::Vec2(getVelocity().x, -1 * getVelocity().y));
  }
}


