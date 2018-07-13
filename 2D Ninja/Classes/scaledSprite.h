#ifndef SCALEDSPRITE__H
#define SCALEDSPRITE__H

#include <string>
#include <iostream>
#include "utilities.h"

#include "drawable.h"

class ScaledSprite : public Drawable {
public:
  ScaledSprite(const cocos2d::Vec2&, const cocos2d::Vec2&, const char*, float minScale, float maxScale);
  ScaledSprite(const ScaledSprite& s);
  ~ScaledSprite();

  bool operator<(const ScaledSprite& rhs) const {
    return scale < rhs.scale;
  }

  void update(float);
  cocos2d::Sprite* getSprite() const { return sprite; };

  float getScale() const { return scale; }

private:
  double scale;
  cocos2d::Sprite *sprite;
  int maxX;
  int minX;
  int maxY;
  int minY;
};
#endif

