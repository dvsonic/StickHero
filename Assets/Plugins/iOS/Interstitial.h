//
//  TestiAd
//
//  Created by ���� on 15/6/28.
//  Copyright (c) 2015�� ����. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <iAd/iAd.h>

@interface Interstitial : UIViewController<ADInterstitialAdDelegate>
{
    ADInterstitialAd *interstitial;
}
+(Interstitial*)getInstance;
-(BOOL) ShowInterstitial;
-(void) HideInterstitial;

@end

