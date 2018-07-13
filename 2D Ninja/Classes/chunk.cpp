#include <iostream>
#include <cmath>
#include "chunk.h"

Chunk::Chunk(const std::string name, float lx, float ly, const cocos2d::Vec2& v, int ox, int oy, int w, int h):
  Drawable( v, cocos2d::Vec2(0, 0)),
  sprite( cocos2d::Sprite::create(name, cocos2d::Rect(ox, oy, w, h))),
  size( sprite->getContentSize().width, sprite->getContentSize().height ),
  lifetime(0.0f),
  isRendered(false)
{
  float theta;
  sprite->setPosition( cocos2d::Point(lx, ly) ); 
  int up;
  int basespeed = 200;
  int flospeed = 100;
  theta = rand() % 360 * (M_PI) / 360;
  float temp = basespeed + rand() % flospeed - flospeed / 2;
  up = rand() % 2 ? 1 : -1;
  setVelocity(cocos2d::Vec2(up*temp * cos(theta), up*temp*sin(theta)));
}

void Chunk::update(float dt) {
  lifetime += dt;
  cocos2d::Point position = sprite->getPosition();
  cocos2d::Vec2 incr = getVelocity() * dt;
  sprite->setPosition(position.x + incr.x, position.y + incr.y );
}
bool Chunk::getIsRemovable() const {
  if(lifetime > 1.0f)
    return true;
  return false;
}

