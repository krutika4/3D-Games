#include "drawable.h"

class MultiSprite: public Drawable
{
public:
  MultiSprite(const cocos2d::Vec2&, const cocos2d::Vec2&,
    const char*, const char*, int);
  MultiSprite(const MultiSprite&);
  cocos2d::Vector<cocos2d::SpriteFrame*> getAnimation (const char*, int) const;
  void update(float);
  cocos2d::Sprite* getSprite() const { return sprite; };
  bool collidedRect(const Drawable*) const;
private:
  cocos2d::Vector<cocos2d::SpriteFrame*> frames;
  cocos2d::Sprite *sprite;
};

