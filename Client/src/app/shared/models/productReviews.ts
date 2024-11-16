import { IReview } from './review';
export interface IProductReviews {

  reviews: IReview[],
  rating: number,
  stars: number[]
}
