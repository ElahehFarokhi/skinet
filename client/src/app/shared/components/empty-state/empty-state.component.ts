import { Component, inject } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { CartService } from '../../../core/services/cart.service';
import { CartItemComponent } from '../../../features/cart/cart-item/cart-item.component';
import { OrderSummaryComponent } from '../order-summary/order-summary.component';

@Component({
  selector: 'app-empty-state',
  standalone: true,
  imports: [
    MatIcon,
    MatButton,
    RouterLink,
    CartItemComponent,
    OrderSummaryComponent,
  ],
  templateUrl: './empty-state.component.html',
  styleUrl: './empty-state.component.scss',
})
export class EmptyStateComponent {
  cartService = inject(CartService);
}
