#ifndef CHUNK__H
#define CHUNK__H
#include "cocos2d.h"
#include "drawable.h"
#include <cstring>

/*
 *  Chunk is general sprite.
 *
 */

class Chunk : public Drawable {
public:
  Chunk(const std::string, float , float , const cocos2d::Vec2& , int , int , int , int);
  ~Chunk() { }
  void virtual update(float);
  cocos2d::Sprite* getSprite() const { return sprite; }
  cocos2d::Vec2 getPos() const { return sprite->getPosition(); }
  bool getIsRemovable() const;
  virtual bool getIsRendered() const { return isRendered; }
  virtual void setIsRendered(const bool r) { isRendered = r; }

private:
  cocos2d::Sprite* sprite;
  cocos2d::Vec2 size;
  float lifetime;
  bool isRendered;

};
#endif


