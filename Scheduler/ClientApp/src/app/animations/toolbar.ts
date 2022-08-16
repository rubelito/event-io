import { trigger, state, style, transition,
    animate, group, query, stagger, keyframes
} from '@angular/animations';

export const SlideInOutAnimation = [
    trigger('slideInOut', [
      transition(':enter', [
        style({height: '0'}),
        animate('200ms ease-in', style({height: '*'}))
      ]),
      transition(':leave', [
        animate('200ms ease-in', style({height: '0'}))
      ])
    ])
]