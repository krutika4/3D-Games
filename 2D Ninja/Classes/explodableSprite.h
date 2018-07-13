#include "drawable.h"
#include "chunk.h"

class ExplodableSprite: public Drawable
{
public:
  ExplodableSprite(const cocos2d::Vec2&, const cocos2d::Vec2&, const char*);
  ExplodableSprite(const ExplodableSprite&);
  ~ExplodableSprite();
  void update(float);
  cocos2d::Sprite* getSprite() const { return sprite; };
  void onCollide();
  virtual void makeChunks(std::list<Drawable*>&);
  float getDistance(const Drawable* b) const;
  bool getIsRemovable() const{ return isRemovable; }
  virtual void setIsRemovable(const bool r) { isRemovable = r; }
  virtual bool getIsExplodable() const { return true; }

private:
  cocos2d::Sprite *sprite;
  bool isRemovable;
  int maxX;
  int minX;
  int maxY;
  int minY;
};

