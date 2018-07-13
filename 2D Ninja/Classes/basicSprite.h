#include "drawable.h"
#include "chunk.h"
class BasicSprite : public Drawable
{
public:
  BasicSprite(const cocos2d::Vec2&, const cocos2d::Vec2&, const char*);
  BasicSprite(const BasicSprite&);
  ~BasicSprite();
  void update(float);
  cocos2d::Sprite* getSprite() const { return sprite; };
  void onCollide();
  float getDistance(const Drawable* b) const;
  bool getIsRemovable() const { return isRemovable; }
  virtual void setIsRemovable(const bool r) { isRemovable = r; }
  virtual bool getIsRendered() const { return isRendered; }
  virtual void setIsRendered(const bool r) { isRendered = r; }
  void reset(const cocos2d::Vec2&);
  bool collidedRect(const Drawable*) const;
private:
  cocos2d::Sprite *sprite;
  bool isRemovable;
  float lifetime;
  bool isRendered;
};

