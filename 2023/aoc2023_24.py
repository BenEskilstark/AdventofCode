import numpy as np

def find_interceptor_parameters(targets, intercept_times):
    # targets is a list of (point, velocity) pairs for the target objects
    # intercept_times is a list of times at which each target will be intercepted

    # Number of targets
    num_targets = len(targets)

    # Initialize the matrices for the linear system
    A = np.empty((num_targets, 3))
    B = np.empty((num_targets, 3))

    for i, ((px, py, pz), (vx, vy, vz)) in enumerate(targets):
        t = intercept_times[i]
        # Fill the matrices with the current information
        A[i, :] = [vx * t - 1, vy * t, vz * t]
        B[i, :] = [px - vx * t, py - vy * t, pz - vz * t]

    # Use least squares to find the best-fit solution for the interceptor's velocity
    velocity, _, _, _ = np.linalg.lstsq(A, -B, rcond=None)

    # Solve for the starting point using the first target and interception time
    first_target_point, _ = targets[0]
    first_intercept_time = intercept_times[0]
    interceptor_start_point = first_target_point - velocity.T * first_intercept_time

    return interceptor_start_point.flatten(), velocity.flatten()

# Example usage
targets = [((19,13,30), (-2,1,-2)), ((18,19,22), (-1,-1,-2)), ((20,25,34), (-2,-2,-4))]
intercept_times = [1, 2, 3]  # Times at which each target is intercepted

interceptor_start, interceptor_velocity = find_interceptor_parameters(targets, intercept_times)
print("Interceptor Start Point:", interceptor_start)
print("Interceptor Velocity:", interceptor_velocity)
