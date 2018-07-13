#ifndef __DRAWABLE_H__
#define __DRAWABLE_H__
#include "cocos2d.h"

class Drawable
{
public:
  Drawable(const cocos2d::Vec2& v, const cocos2d::Vec2& p):
    velocity(v), 
    origPosition(p),
    viewSize(cocos2d::Director::getInstance()->getVisibleSize()),
    origin(cocos2d::Director::getInstance()->getVisibleOrigin()) {}
  Drawable(const Drawable& s):
    velocity(s.velocity), 
    viewSize(cocos2d::Director::getInstance()->getVisibleSize()),
    origin(cocos2d::Director::getInstance()->getVisibleOrigin()) {}
  virtual ~Drawable() {}
  virtual void update(float) = 0;
  virtual cocos2d::Sprite* getSprite() const = 0;
  cocos2d::Vec2 getVelocity() const { return velocity; }
  cocos2d::Vec2 getOrigPosition() const { return origPosition; }
  void setVelocity(const cocos2d::Vec2& v) { velocity.set(v); }
  float VelX() { return velocity.x; }
  void VelX(float x) { velocity.x = x; }
  float VelY() { return velocity.y; }
  void VelY(float y) { velocity.y = y; }

  cocos2d::Size getViewSize() const { return viewSize; }
  virtual bool getIsRemovable() const{ return false; }
  virtual void setIsRemovable(const bool r) { }
  virtual bool getIsRendered() const { return true; }
  virtual void setIsRendered(const bool r) {  }
  virtual bool getIsExplodable() const { return false; }

  virtual void makeChunks(std::list<Drawable*>& ){}
private:
  cocos2d::Vec2 velocity;
  cocos2d::Vec2 origPosition;
  cocos2d::Size viewSize;
  cocos2d::Point origin;
  std::list<Drawable*> chunkList;
  bool isRemovable;
};
#endif
