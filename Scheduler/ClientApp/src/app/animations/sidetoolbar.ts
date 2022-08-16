import { trigger, state, style, transition,
    animate, group, query, stagger, keyframes
} from '@angular/animations';

export const SlideSideInOutAnimation = [
    trigger('slideSideInOut', [
      transition(':enter', [
        style({left: '-280px'}),
        animate('350ms ease-in', style({left: '0px'}))
      ]),
      transition(':leave', [
        animate('350ms ease-in', style({left: '-280px'}))
      ])
    ])
]