#include "explodableSprite.h"
#include "drawable.h"
#include <iostream>

ExplodableSprite::ExplodableSprite(const cocos2d::Vec2& v, const cocos2d::Vec2& p, const char* imgFile) :
  Drawable(v,p),
  sprite(cocos2d::Sprite::create(imgFile)),
  isRemovable(false),
  maxX(400),
  minX(100),
  maxY(300),
  minY(100)
{
  sprite->setPosition(p.x, p.y);
}

ExplodableSprite::ExplodableSprite(const ExplodableSprite& e) :
  Drawable(e),
  sprite(e.sprite)
{
}

ExplodableSprite::~ExplodableSprite()
{
}

void ExplodableSprite::update(float dt)
{
  cocos2d::Point position = sprite->getPosition();
  cocos2d::Vec2 incr = getVelocity() * dt;
  sprite->setPosition(position.x + incr.x, position.y + incr.y);
}

void ExplodableSprite::makeChunks(std::list<Drawable*>& sprites) {
  
    for (int i = 0;i<4;i++) {
      for (int j = 0;j<4;j++) {
        sprites.push_back(new Chunk("enemy.png", sprite->getPosition().x, sprite->getPosition().y, cocos2d::Vec2(100, 100), i * 15, j * 8, 15, 8));
        

      }
    }
}


