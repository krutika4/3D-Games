#include "multiSprite.h"
#include "drawable.h"
MultiSprite::MultiSprite(const cocos2d::Vec2& v, const cocos2d::Vec2& p, 
                         const char* plistFile, const char* imgFile, int numberOfFrames) :
  Drawable(v,p),
  frames(),
  sprite()
{
  
  cocos2d::SpriteFrameCache::getInstance()->addSpriteFramesWithFile(plistFile);
  frames = getAnimation(imgFile, numberOfFrames);
  sprite = cocos2d::Sprite::createWithSpriteFrame(frames.front());
  sprite->setPosition(p.x, p.y);
  cocos2d::Animation*
    animation = cocos2d::Animation::createWithSpriteFrames(frames, 1.0f / numberOfFrames);
  sprite->runAction(
    cocos2d::RepeatForever::create(cocos2d::Animate::create(animation))
    );
}

MultiSprite::MultiSprite(const MultiSprite& m) :
  Drawable(m),
  frames(m.frames),
  sprite(m.sprite)
{}

cocos2d::Vector<cocos2d::SpriteFrame*> MultiSprite::getAnimation (const char *format, int count) const{
  cocos2d::SpriteFrameCache*
    spritecache = cocos2d::SpriteFrameCache::getInstance();
  cocos2d::Vector<cocos2d::SpriteFrame*> animFrames;
  char str[100];
  for (int i = 1; i <= count; i++) {
    sprintf(str, format, i);
    animFrames.pushBack(spritecache->getSpriteFrameByName(str));
  }
  return animFrames;
}

void MultiSprite::update(float dt)
{

  cocos2d::Point position = sprite->getPosition();
  cocos2d::Vec2 incr = getVelocity() * dt;
  sprite->setPosition(position.x + incr.x, position.y + incr.y);

  cocos2d::Point location = sprite->getPosition();
  if (location.x >512) {
    sprite->setPosition(getOrigPosition());
  }
}

bool MultiSprite::collidedRect(const Drawable* otherSprite) const {
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
